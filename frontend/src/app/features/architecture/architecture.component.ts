import { Component, inject, output } from '@angular/core';
import { ExamStore } from '../../core/services/exam.store';
import { IconComponent } from '../../shared/icon.component';

@Component({
  selector: 'app-architecture',
  standalone: true,
  imports: [IconComponent],
  templateUrl: './architecture.component.html',
})
export class ArchitectureComponent {
  readonly store = inject(ExamStore);
  readonly selectRequirement = output<number>();
}
