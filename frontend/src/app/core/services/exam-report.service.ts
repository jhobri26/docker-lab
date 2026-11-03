import { inject, Injectable, signal } from '@angular/core';
import { ExamApiService } from './exam-api.service';
import { ExamStore } from './exam.store';

@Injectable({ providedIn: 'root' })
export class ExamReportService {
  private readonly api = inject(ExamApiService);
  private readonly store = inject(ExamStore);
  readonly generating = signal(false);
  readonly error = signal('');
  readonly message = signal('');

  async generate(): Promise<void> {
    if (this.generating()) return;
    this.generating.set(true);
    this.error.set('');
    this.message.set('');
    try {
      await this.store.refresh();
      const { blob, fileName } = await this.api.downloadFinalReport();
      const url = URL.createObjectURL(blob);
      const anchor = document.createElement('a');
      anchor.href = url;
      anchor.download = fileName;
      document.body.appendChild(anchor);
      try { anchor.click(); }
      finally {
        anchor.remove();
        setTimeout(() => URL.revokeObjectURL(url), 60_000);
      }
      this.message.set('PDF generado. La descarga se ha iniciado.');
    } catch (error) {
      this.error.set(error instanceof Error ? error.message : 'No se pudo generar el informe. Inténtalo nuevamente.');
    } finally {
      this.generating.set(false);
    }
  }
}
