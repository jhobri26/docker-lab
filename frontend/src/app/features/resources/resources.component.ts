import { Component, inject, output } from '@angular/core';
import { FinalReportComponent } from '../reports/final-report.component';
import { ExamStore } from '../../core/services/exam.store';
import { IconComponent } from '../../shared/icon.component';

@Component({
  selector: 'app-resources',
  standalone: true,
  imports: [IconComponent, FinalReportComponent],
  templateUrl: './resources.component.html',
})
export class ResourcesComponent {
  readonly store = inject(ExamStore);
  readonly editProfile = output<void>();
}
