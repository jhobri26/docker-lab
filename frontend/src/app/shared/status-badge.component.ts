import { Component, input } from '@angular/core';
import { RequirementStatus, STATUS_LABELS } from '../core/models/exam.models';

@Component({
  selector: 'app-status-badge',
  standalone: true,
  template: '<span class="status" [attr.data-status]="status()">{{ labels[status()] }}</span>',
  styles: ':host { display: contents; }',
})
export class StatusBadgeComponent {
  readonly status = input.required<RequirementStatus>();
  readonly labels = STATUS_LABELS;
}
