import { Component, inject } from '@angular/core';
import { ExamStore } from '../../core/services/exam.store';
import { ExamReportService } from '../../core/services/exam-report.service';
import { IconComponent } from '../../shared/icon.component';

@Component({
  selector: 'app-final-report',
  standalone: true,
  imports: [IconComponent],
  templateUrl: './final-report.component.html',
  styleUrl: './final-report.component.css',
})
export class FinalReportComponent {
  readonly store = inject(ExamStore);
  readonly report = inject(ExamReportService);
}
