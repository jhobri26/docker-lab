import { Component, HostListener, inject, input, OnInit, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { Evidence } from '../../core/models/exam.models';
import { ExamApiService } from '../../core/services/exam-api.service';
import { EvidenceContentService } from '../../core/services/evidence-content.service';
import { DialogFocusDirective } from '../../shared/dialog-focus.directive';
import { IconComponent } from '../../shared/icon.component';
import { RichTextEditorComponent } from '../../shared/rich-text-editor.component';

@Component({
  selector: 'app-evidence-edit-dialog',
  standalone: true,
  imports: [FormsModule, DialogFocusDirective, IconComponent, RichTextEditorComponent],
  templateUrl: './evidence-edit-dialog.component.html',
})
export class EvidenceEditDialogComponent implements OnInit {
  private readonly api = inject(ExamApiService);
  private readonly content = inject(EvidenceContentService);
  readonly evidence = input.required<Evidence>();
  readonly closed = output<void>();
  readonly saved = output<Evidence>();
  readonly busy = signal(false);
  readonly error = signal('');
  title = '';
  description = '';

  ngOnInit(): void {
    this.title = this.evidence().title;
    this.description = this.content.toHtml(
      this.evidence().description,
      this.evidence().descriptionFormat ?? 'markdown',
    );
  }

  @HostListener('document:keydown.escape') close(): void {
    if (!this.busy()) this.closed.emit();
  }

  async save(): Promise<void> {
    if (this.busy()) return;
    if (!this.title.trim()) {
      this.error.set('Escribe un título para la evidencia.');
      return;
    }
    if (this.description.length > 20000) {
      this.error.set('La descripción es demasiado extensa. Reduce su contenido antes de guardar.');
      return;
    }
    this.busy.set(true);
    this.error.set('');
    try {
      const result = await this.api.updateEvidence(this.evidence().id, {
        title: this.title.trim(),
        description: this.description,
        descriptionFormat: 'html',
      });
      this.saved.emit(result);
    } catch (error) {
      this.error.set((error as Error).message);
    } finally {
      this.busy.set(false);
    }
  }
}
