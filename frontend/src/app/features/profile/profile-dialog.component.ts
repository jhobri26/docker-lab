import { Component, HostListener, inject, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { ExamApiService } from '../../core/services/exam-api.service';
import { ExamStore } from '../../core/services/exam.store';
import { NotificationService } from '../../core/services/notification.service';

@Component({
  selector: 'app-profile-dialog',
  standalone: true,
  imports: [FormsModule],
  templateUrl: './profile-dialog.component.html',
})
export class ProfileDialogComponent {
  private readonly api = inject(ExamApiService);
  private readonly store = inject(ExamStore);
  private readonly notifications = inject(NotificationService);
  readonly closed = output<void>();
  readonly busy = signal(false);
  student = this.store.data()?.profile.student ?? '';
  repositoryUrl = this.store.data()?.profile.repositoryUrl ?? '';

  @HostListener('document:keydown.escape') close(): void {
    if (!this.busy()) this.closed.emit();
  }

  async save(): Promise<void> {
    this.busy.set(true);
    try {
      await this.api.updateProfile({
        student: this.student,
        repositoryUrl: this.repositoryUrl,
      });
      await this.store.refresh();
      this.closed.emit();
      this.notifications.show('Datos de presentación guardados.');
    } catch (error) {
      this.notifications.show((error as Error).message);
    } finally {
      this.busy.set(false);
    }
  }
}
