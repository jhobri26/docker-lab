import { Component, computed, inject, input } from '@angular/core';
import { EvidenceContentService } from '../core/services/evidence-content.service';

@Component({
  selector: 'app-rich-text',
  standalone: true,
  template: `
    <div class="rich-content" [class.rich-content--compact]="compact()" [innerHTML]="html()"></div>
  `,
})
export class RichTextComponent {
  private readonly content = inject(EvidenceContentService);
  readonly value = input('');
  readonly format = input<'markdown' | 'html'>('markdown');
  readonly compact = input(false);
  readonly html = computed(() => this.content.toHtml(this.value(), this.format()));
}
