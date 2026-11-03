import { Component, computed, inject, output, signal } from '@angular/core';
import { FormsModule } from '@angular/forms';
import { FinalReportComponent } from '../reports/final-report.component';
import { ExamStore } from '../../core/services/exam.store';
import { StatusBadgeComponent } from '../../shared/status-badge.component';
import { IconComponent, IconName } from '../../shared/icon.component';

@Component({
  selector: 'app-overview',
  standalone: true,
  imports: [FormsModule, StatusBadgeComponent, IconComponent, FinalReportComponent],
  templateUrl: './overview.component.html',
  styleUrl: './overview.component.css',
})
export class OverviewComponent {
  readonly store = inject(ExamStore);
  readonly selectRequirement = output<number>();
  readonly editProfile = output<void>();
  readonly search = signal('');
  readonly filter = signal('todos');
  readonly caseIcons: Record<number, IconName> = {
    1: 'book',
    2: 'box',
    3: 'network',
    4: 'layers',
    5: 'code',
    6: 'upload',
    7: 'grid',
    8: 'terminal',
    9: 'shield',
    10: 'activity',
  };
  readonly filters = [
    { id: 'todos', name: 'Todos' },
    { id: 'pendiente', name: 'Pendientes' },
    { id: 'en-progreso', name: 'En progreso' },
    { id: 'listo', name: 'Listos' },
  ];
  readonly filtered = computed(
    () =>
      this.store
        .data()
        ?.requirements.filter(
          (requirement) =>
            (this.filter() === 'todos' || requirement.progress.status === this.filter()) &&
            `${requirement.id} ${requirement.title} ${requirement.summary}`
              .toLocaleLowerCase('es')
              .includes(this.search().toLocaleLowerCase('es')),
        ) ?? [],
  );

  filterCount(status: string): number {
    const requirements = this.store.data()?.requirements ?? [];
    return status === 'todos'
      ? requirements.length
      : requirements.filter((item) => item.progress.status === status).length;
  }
}
