import { Component, HostListener, input, output, signal } from '@angular/core';
import { CommonModule } from '@angular/common';
import { Evidence } from '../core/models/exam.models';
import { DialogFocusDirective } from './dialog-focus.directive';
import { IconComponent } from './icon.component';
import { RichTextComponent } from './rich-text.component';

@Component({
  selector: 'app-image-dialog',
  standalone: true,
  imports: [CommonModule, DialogFocusDirective, IconComponent, RichTextComponent],
  templateUrl: './image-dialog.component.html',
})
export class ImageDialogComponent {
  readonly evidence = input.required<Evidence>();
  readonly closed = output<void>();
  readonly editRequested = output<Evidence>();
  readonly zoom = signal(1);

  changeZoom(delta: number): void {
    this.zoom.update((current) => Math.max(1, Math.min(3, current + delta)));
  }

  @HostListener('document:keydown.escape') close(): void {
    this.closed.emit();
  }
}
