import { Component, HostListener, inject, signal } from '@angular/core';
import { PortalView } from './core/models/exam.models';
import { ExamStore } from './core/services/exam.store';
import { NotificationService } from './core/services/notification.service';
import { ArchitectureComponent } from './features/architecture/architecture.component';
import { OverviewComponent } from './features/overview/overview.component';
import { ProfileDialogComponent } from './features/profile/profile-dialog.component';
import { RequirementComponent } from './features/requirements/requirement.component';
import { ResourcesComponent } from './features/resources/resources.component';
import { IconComponent } from './shared/icon.component';

@Component({
  selector: 'app-root',
  standalone: true,
  imports: [
    ArchitectureComponent,
    OverviewComponent,
    ProfileDialogComponent,
    RequirementComponent,
    ResourcesComponent,
    IconComponent,
  ],
  templateUrl: './app.component.html',
})
export class AppComponent {
  readonly store = inject(ExamStore);
  readonly notifications = inject(NotificationService);
  readonly view = signal<PortalView>('overview');
  readonly selectedId = signal(1);
  readonly profileOpen = signal(false);
  readonly sidebarOpen = signal(false);

  constructor() {
    void this.store.load();
  }

  open(id: number): void {
    this.selectedId.set(id);
    this.navigate('case');
  }

  navigate(view: PortalView): void {
    this.view.set(view);
    this.sidebarOpen.set(false);
    window.scrollTo({ top: 0, behavior: 'smooth' });
  }

  @HostListener('document:keydown.escape') closeSidebar(): void {
    this.sidebarOpen.set(false);
  }
}
