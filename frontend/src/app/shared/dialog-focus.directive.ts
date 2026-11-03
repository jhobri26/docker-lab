import {
  AfterViewInit,
  Directive,
  ElementRef,
  HostListener,
  inject,
  OnDestroy,
} from '@angular/core';

@Directive({ selector: '[appDialogFocus]', standalone: true })
export class DialogFocusDirective implements AfterViewInit, OnDestroy {
  private readonly element = inject<ElementRef<HTMLElement>>(ElementRef);
  private readonly previousFocus = document.activeElement as HTMLElement | null;
  private controls(): HTMLElement[] {
    return Array.from(
      this.element.nativeElement.querySelectorAll<HTMLElement>(
        'button:not([disabled]), a[href], input:not([disabled]), textarea:not([disabled]), select:not([disabled]), [contenteditable="true"], [tabindex="0"]',
      ),
    ).filter((item) => item.offsetParent !== null);
  }
  ngAfterViewInit(): void {
    queueMicrotask(() =>
      (
        this.element.nativeElement.querySelector<HTMLElement>('[data-autofocus]') ??
        this.controls()[0]
      )?.focus(),
    );
  }
  @HostListener('keydown', ['$event']) onTab(event: Event): void {
    if (!(event instanceof KeyboardEvent) || event.key !== 'Tab') return;
    const controls = this.controls();
    const first = controls[0],
      last = controls.at(-1);
    if (event.shiftKey && document.activeElement === first) {
      event.preventDefault();
      last?.focus();
    }
    if (!event.shiftKey && document.activeElement === last) {
      event.preventDefault();
      first?.focus();
    }
  }
  ngOnDestroy(): void {
    if (this.previousFocus?.isConnected) this.previousFocus.focus();
  }
}
