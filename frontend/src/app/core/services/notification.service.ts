import { DestroyRef, inject, Injectable, signal } from '@angular/core';

@Injectable({ providedIn: 'root' })
export class NotificationService {
  private readonly text = signal('');
  private timer?: ReturnType<typeof setTimeout>;
  readonly message = this.text.asReadonly();

  constructor() {
    inject(DestroyRef).onDestroy(() => clearTimeout(this.timer));
  }

  show(message: string): void {
    clearTimeout(this.timer);
    this.text.set(message);
    this.timer = setTimeout(() => this.clear(), 5000);
  }

  clear(): void {
    this.text.set('');
  }

  async copy(code: string): Promise<void> {
    try {
      await navigator.clipboard.writeText(code);
      this.show('Comandos copiados. Revisa los valores antes de ejecutarlos.');
    } catch {
      this.show('Selecciona el bloque y cópialo con Ctrl+C.');
    }
  }
}
