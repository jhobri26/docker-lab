import { computed, inject, Injectable, signal } from '@angular/core';
import { ExamData, ServiceStatus } from '../models/exam.models';
import { ExamApiService } from './exam-api.service';

@Injectable({ providedIn: 'root' })
export class ExamStore {
  private readonly api = inject(ExamApiService);
  private readonly examData = signal<ExamData | null>(null);
  private readonly serviceData = signal<ServiceStatus | null>(null);

  readonly data = this.examData.asReadonly();
  readonly services = this.serviceData.asReadonly();
  readonly loading = signal(false);
  readonly error = signal('');
  readonly ready = computed(
    () => this.data()?.requirements.filter((x) => x.progress.status === 'listo').length ?? 0,
  );
  readonly totalEvidence = computed(
    () => this.data()?.requirements.reduce((n, x) => n + x.evidence.length, 0) ?? 0,
  );
  readonly totalCases = computed(() => this.data()?.requirements.length ?? 0);
  readonly reportReady = computed(() => this.totalCases() === 10 && this.ready() === 10);
  readonly inProgress = computed(
    () =>
      this.data()?.requirements.filter((item) => item.progress.status === 'en-progreso').length ??
      0,
  );
  readonly completion = computed(() =>
    this.totalCases() ? Math.round((this.ready() / this.totalCases()) * 100) : 0,
  );
  readonly nextRequirement = computed(() => {
    const requirements = this.data()?.requirements ?? [];
    return (
      requirements.find((item) => item.progress.status === 'en-progreso') ??
      requirements.find((item) => item.progress.status === 'pendiente') ??
      requirements[0]
    );
  });
  readonly studentInitials = computed(
    () =>
      (this.data()?.profile.student ?? '')
        .split(/\s+/)
        .filter(Boolean)
        .slice(0, 2)
        .map((word) => word[0])
        .join('') || 'AL',
  );
  readonly studentDisplayName = computed(() =>
    (this.data()?.profile.student ?? 'Alumno')
      .split(/\s+/)
      .slice(0, 2)
      .map((word) => word.charAt(0) + word.slice(1).toLocaleLowerCase('es'))
      .join(' '),
  );

  async load(): Promise<void> {
    this.loading.set(true);
    this.error.set('');
    try {
      await this.refresh();
      await this.checkServices();
    } catch (error) {
      this.error.set((error as Error).message);
    } finally {
      this.loading.set(false);
    }
  }

  async refresh(): Promise<void> {
    this.examData.set(await this.api.getExam());
  }

  async checkServices(): Promise<void> {
    try {
      this.serviceData.set(await this.api.getServices());
    } catch {
      this.serviceData.set(null);
    }
  }
}
