import { Component, input } from '@angular/core';

const ICON_PATHS = {
  edit: 'm16 3 5 5M4 15 15 4a3.5 3.5 0 0 1 5 5L9 20l-6 1 1-6Z',
  eye: 'M2 12s3-7 10-7 10 7 10 7-3 7-10 7S2 12 2 12Zm14 0a4 4 0 1 1-8 0 4 4 0 0 1 8 0Z',
  zoomIn: 'M21 21l-5-5M18 10a8 8 0 1 1-16 0 8 8 0 0 1 16 0ZM6 10h8m-4-4v8',
  zoomOut: 'M21 21l-5-5M18 10a8 8 0 1 1-16 0 8 8 0 0 1 16 0ZM6 10h8',
  list: 'M9 6h12M9 12h12M9 18h12M3 6h.01M3 12h.01M3 18h.01',
  listOrdered: 'M10 6h11M10 12h11M10 18h11M3 3h2v5M3 8h4M3 13c0-3 5-3 4 0l-4 5h4',
  quote: 'M3 4h7v8H6c0 4-1 6-3 7m11-15h7v8h-4c0 4-1 6-3 7',
  undo: 'M3 10h12a6 6 0 0 1 0 12M3 10l6-6M3 10l6 6',
  redo: 'M21 10H9a6 6 0 0 0 0 12m12-12-6-6m6 6-6 6',
  clearFormat: 'M3 4h15M10 4l-4 14m4 0h3m2-5 6 6m-6 0 6-6',
  grid: 'M3 3h7v7H3z M14 3h7v7h-7z M3 14h7v7H3z M14 14h7v7h-7z',
  box: 'm12 3 9 5v9l-9 5-9-5V8l9-5Zm0 9 9-4M12 12 3 8m9 4v10M7.5 5.5l9 5',
  layers: 'm12 3 10 5-10 5L2 8l10-5ZM2 12l10 5 10-5M2 16l10 5 10-5',
  folder: 'M3 7V5a2 2 0 0 1 2-2h5l2 3h7a2 2 0 0 1 2 2v11a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V7Z',
  arrowRight: 'M4 12h16m-6-6 6 6-6 6',
  arrowUpRight: 'M6 18 18 6M6 6h12v12',
  download: 'M12 3v12m-5-5 5 5 5-5M4 16v4a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-4',
  check: 'm5 12 4 4L19 6',
  checkCircle: 'M22 11v1a10 10 0 1 1-6-9m-7 8 3 3L22 4',
  clock: 'M22 12a10 10 0 1 1-20 0 10 10 0 0 1 20 0ZM12 6v6l4 2',
  image:
    'M5 3h14a2 2 0 0 1 2 2v14a2 2 0 0 1-2 2H5a2 2 0 0 1-2-2V5a2 2 0 0 1 2-2ZM3 16l5-5 4 4 4-6 5 7M9 7h.01',
  search: 'M21 21l-5-5M18 10a8 8 0 1 1-16 0 8 8 0 0 1 16 0Z',
  terminal:
    'm5 7 5 5-5 5m8 0h6M4 3h16a1 1 0 0 1 1 1v16a1 1 0 0 1-1 1H4a1 1 0 0 1-1-1V4a1 1 0 0 1 1-1Z',
  network: 'M9 3h6v6H9zM2 16h6v6H2zM16 16h6v6h-6zM12 9v4M5 16v-3h14v3',
  database:
    'M21 5c0 2-4 3-9 3S3 7 3 5s4-3 9-3 9 1 9 3Zm0 0v14c0 2-4 3-9 3s-9-1-9-3V5m0 7c0 2 4 3 9 3s9-1 9-3',
  upload: 'M12 16V3m-5 5 5-5 5 5M4 16v4a1 1 0 0 0 1 1h14a1 1 0 0 0 1-1v-4',
  chevronRight: 'm9 5 7 7-7 7',
  code: 'm8 6-6 6 6 6m8-12 6 6-6 6m-3-15-2 18',
  shield: 'M12 2 3 6v6c0 5 9 10 9 10s9-5 9-10V6l-9-4Zm-4 10 3 3 5-6',
  refresh: 'M20 7a9 9 0 0 0-15-2L2 8m0-5v5h5m-3 9a9 9 0 0 0 15 2l3-3m0 5v-5h-5',
  menu: 'M3 6h18M3 12h18M3 18h18',
  close: 'm6 6 12 12M6 18 18 6',
  file: 'M14 2H5a2 2 0 0 0-2 2v16a2 2 0 0 0 2 2h14a2 2 0 0 0 2-2V9l-7-7Zm0 0v7h7M7 13h10M7 17h7',
  chevronLeft: 'm15 5-7 7 7 7',
  book: 'M12 5c-3-3-7-3-10-2v17c3-1 7-1 10 2m0-17c3-3 7-3 10-2v17c-3-1-7-1-10 2V5Z',
  activity: 'M2 12h5l3-9 4 18 3-9h5',
  play: 'm8 4 13 8-13 8V4Z',
  trophy:
    'M8 3h8v6a4 4 0 0 1-8 0V3Zm0 2H4v3a4 4 0 0 0 4 4m8-7h4v3a4 4 0 0 1-4 4m-4 1v5m-4 3h8m-6-3h4v3h-4z',
  user: 'M16 7a4 4 0 1 1-8 0 4 4 0 0 1 8 0ZM4 21v-2a8 8 0 0 1 16 0v2',
  info: 'M22 12a10 10 0 1 1-20 0 10 10 0 0 1 20 0ZM12 11v6m0-10h.01',
} as const;

export type IconName = keyof typeof ICON_PATHS;

@Component({
  selector: 'app-icon',
  standalone: true,
  template: `
    <svg
      viewBox="0 0 24 24"
      fill="none"
      stroke="currentColor"
      stroke-width="1.7"
      stroke-linecap="round"
      stroke-linejoin="round"
      aria-hidden="true"
      focusable="false"
      [attr.width]="size()"
      [attr.height]="size()"
    >
      <path [attr.d]="paths[name()]" />
    </svg>
  `,
  styles:
    ':host { display: inline-flex; align-items: center; justify-content: center; flex-shrink: 0; } svg { display: block; }',
})
export class IconComponent {
  readonly name = input<IconName>('box');
  readonly size = input(20);
  readonly paths = ICON_PATHS;
}
