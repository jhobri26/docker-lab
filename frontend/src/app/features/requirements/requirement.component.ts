import { CommonModule } from '@angular/common';
import {
  Component,
  computed,
  effect,
  ElementRef,
  inject,
  input,
  output,
  signal,
  ViewChild,
} from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Evidence, RequirementStatus } from '../../core/models/exam.models';
import { ExamApiService } from '../../core/services/exam-api.service';
import { ExamStore } from '../../core/services/exam.store';
import { NotificationService } from '../../core/services/notification.service';
import { ImageDialogComponent } from '../../shared/image-dialog.component';
import { StatusBadgeComponent } from '../../shared/status-badge.component';
import { IconComponent } from '../../shared/icon.component';
import { RichTextComponent } from '../../shared/rich-text.component';
import { RichTextEditorComponent } from '../../shared/rich-text-editor.component';
import { EvidenceEditDialogComponent } from './evidence-edit-dialog.component';

@Component({
  selector: 'app-requirement',
  standalone: true,
  imports: [
    CommonModule,
    FormsModule,
    ImageDialogComponent,
    StatusBadgeComponent,
    IconComponent,
    RichTextComponent,
    RichTextEditorComponent,
    EvidenceEditDialogComponent,
  ],
  templateUrl: './requirement.component.html',
})
export class RequirementComponent {
  private readonly api = inject(ExamApiService);
  readonly store = inject(ExamStore);
  readonly notifications = inject(NotificationService);
  readonly requirementId = input.required<number>();
  readonly selectRequirement = output<number>();
  readonly back = output<void>();
  readonly selected = computed(() =>
    this.store.data()?.requirements.find((x) => x.id === this.requirementId()),
  );
  readonly busy = signal(false);
  readonly lightbox = signal<Evidence | null>(null);
  readonly editingEvidence = signal<Evidence | null>(null);
  private progressId?: number;
  private lastNotes = '';
  private lastStatus: RequirementStatus = 'pendiente';
  @ViewChild('fileInput') private fileInput?: ElementRef<HTMLInputElement>;
  notes = '';
  status: RequirementStatus = 'pendiente';
  title = '';
  description = '';
  file: File | null = null;

  constructor() {
    effect(() => {
      const requirement = this.selected();
      if (requirement) {
        if (
          requirement.id !== this.progressId ||
          (this.notes === this.lastNotes && this.status === this.lastStatus)
        ) {
          this.notes = requirement.progress.notes;
          this.status = requirement.progress.status;
        }
        this.progressId = requirement.id;
        this.lastNotes = requirement.progress.notes;
        this.lastStatus = requirement.progress.status;
      }
    });
    effect(() => {
      this.requirementId();
      this.resetUpload();
      this.lightbox.set(null);
      this.editingEvidence.set(null);
    });
  }

  async saveProgress(): Promise<void> {
    this.busy.set(true);
    try {
      await this.api.updateProgress(this.requirementId(), {
        status: this.status,
        notes: this.notes,
      });
      await this.store.refresh();
      this.notifications.show('Estado y notas guardados en SQL Server.');
    } catch (error) {
      this.notifications.show((error as Error).message);
    } finally {
      this.busy.set(false);
    }
  }

  pickFile(event: Event): void {
    this.file = (event.target as HTMLInputElement).files?.[0] ?? null;
  }

  async upload(): Promise<void> {
    if (this.busy()) return;
    if (!this.file || !this.title.trim()) {
      this.notifications.show('Selecciona una imagen y escribe un título.');
      return;
    }
    if (this.file.size > 8 * 1024 * 1024) {
      this.notifications.show('La imagen debe pesar como máximo 8 MB.');
      return;
    }
    if (this.description.length > 20000) {
      this.notifications.show(
        'La descripción es demasiado extensa. Reduce su contenido antes de guardar.',
      );
      return;
    }
    this.busy.set(true);
    try {
      await this.api.uploadEvidence(this.requirementId(), this.file, this.title, this.description);
      await this.store.refresh();
      this.resetUpload();
      this.notifications.show('Evidencia guardada. Ya puedes abrirla en la galería.');
    } catch (error) {
      this.notifications.show((error as Error).message);
    } finally {
      this.busy.set(false);
    }
  }

  async removeEvidence(evidence: Evidence): Promise<void> {
    if (!window.confirm(`¿Eliminar la evidencia «${evidence.title}»?`)) return;
    this.busy.set(true);
    try {
      await this.api.deleteEvidence(evidence.id);
      await this.store.refresh();
      this.notifications.show('Evidencia eliminada.');
    } catch (error) {
      this.notifications.show((error as Error).message);
    } finally {
      this.busy.set(false);
    }
  }

  async openEvidence(evidence: Evidence): Promise<void> {
    this.lightbox.set(evidence);
    try {
      const latest = await this.api.getEvidence(evidence.id);
      if (this.lightbox()?.id === evidence.id) this.lightbox.set(latest);
    } catch (error) {
      this.notifications.show((error as Error).message);
    }
  }

  editEvidence(evidence: Evidence): void {
    this.lightbox.set(null);
    this.editingEvidence.set(evidence);
  }

  async evidenceSaved(evidence: Evidence): Promise<void> {
    this.editingEvidence.set(null);
    try {
      await this.store.refresh();
      this.lightbox.set(evidence);
      this.notifications.show(
        'Evidencia actualizada. El título y la descripción quedaron guardados.',
      );
    } catch (error) {
      this.notifications.show((error as Error).message);
    }
  }

  private resetUpload(): void {
    this.file = null;
    this.title = '';
    this.description = '';
    if (this.fileInput) this.fileInput.nativeElement.value = '';
  }
}
