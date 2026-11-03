import {
  AfterViewInit,
  Component,
  computed,
  effect,
  ElementRef,
  inject,
  input,
  OnDestroy,
  output,
  signal,
  ViewChild,
} from '@angular/core';
import { Editor } from '@tiptap/core';
import StarterKit from '@tiptap/starter-kit';
import Highlight from '@tiptap/extension-highlight';
import { EvidenceContentService } from '../core/services/evidence-content.service';
import { IconComponent } from './icon.component';

type EditorCommand =
  | 'bold'
  | 'italic'
  | 'underline'
  | 'highlight'
  | 'heading'
  | 'bulletList'
  | 'orderedList'
  | 'codeBlock'
  | 'blockquote'
  | 'undo'
  | 'redo'
  | 'clear';

@Component({
  selector: 'app-rich-text-editor',
  standalone: true,
  imports: [IconComponent],
  templateUrl: './rich-text-editor.component.html',
})
export class RichTextEditorComponent implements AfterViewInit, OnDestroy {
  private readonly content = inject(EvidenceContentService);
  readonly value = input('');
  readonly valueChange = output<string>();
  readonly disabled = input(false);
  readonly label = input('Descripción de la evidencia');
  readonly placeholder = input(
    'Explica qué demuestra la captura y resalta los resultados importantes.',
  );
  private readonly revision = signal(0);
  readonly empty = computed(() => {
    this.revision();
    return this.editor?.isEmpty ?? true;
  });
  readonly characters = computed(() => {
    this.revision();
    return this.editor?.getText().length ?? 0;
  });
  @ViewChild('editorHost', { static: true }) private host!: ElementRef<HTMLDivElement>;
  private editor?: Editor;
  private lastValue = '';

  constructor() {
    effect(() => {
      const value = this.value();
      const disabled = this.disabled();
      if (!this.editor) return;
      if (this.editor.isEditable === disabled) this.editor.setEditable(!disabled, false);
      if (value !== this.lastValue) {
        this.lastValue = value;
        this.editor.commands.setContent(this.content.toHtml(value, 'html'), { emitUpdate: false });
        this.revision.update((current) => current + 1);
      }
    });
  }

  ngAfterViewInit(): void {
    this.lastValue = this.value();
    this.editor = new Editor({
      element: this.host.nativeElement,
      extensions: [
        StarterKit.configure({
          heading: { levels: [1, 2, 3] },
          link: { openOnClick: false },
          underline: {},
        }),
        Highlight.configure({ multicolor: false }),
      ],
      content: this.content.toHtml(this.value(), 'html'),
      editable: !this.disabled(),
      editorProps: {
        attributes: {
          role: 'textbox',
          'aria-multiline': 'true',
          'aria-label': this.label(),
          'data-placeholder': this.placeholder(),
        },
        transformPastedHTML: (html) => this.content.toHtml(html, 'html'),
        handlePaste: (_view, event) => {
          const text = event.clipboardData?.getData('text/plain') ?? '';
          const html = event.clipboardData?.getData('text/html');
          if (!html && this.content.looksLikeMarkdown(text)) {
            this.editor?.chain().focus().insertContent(this.content.toHtml(text, 'markdown')).run();
            return true;
          }
          return false;
        },
      },
      onUpdate: ({ editor }) => {
        this.lastValue = editor.isEmpty ? '' : editor.getHTML();
        this.valueChange.emit(this.lastValue);
        this.revision.update((current) => current + 1);
      },
      onSelectionUpdate: () => this.revision.update((current) => current + 1),
    });
    this.revision.update((current) => current + 1);
  }

  active(command: string): boolean {
    this.revision();
    return command === 'heading'
      ? (this.editor?.isActive('heading', { level: 2 }) ?? false)
      : (this.editor?.isActive(command) ?? false);
  }

  run(command: EditorCommand): void {
    if (!this.editor || this.disabled()) return;
    const chain = this.editor.chain().focus();
    switch (command) {
      case 'bold':
        chain.toggleBold().run();
        break;
      case 'italic':
        chain.toggleItalic().run();
        break;
      case 'underline':
        chain.toggleUnderline().run();
        break;
      case 'highlight':
        chain.toggleHighlight().run();
        break;
      case 'heading':
        chain.toggleHeading({ level: 2 }).run();
        break;
      case 'bulletList':
        chain.toggleBulletList().run();
        break;
      case 'orderedList':
        chain.toggleOrderedList().run();
        break;
      case 'codeBlock':
        chain.toggleCodeBlock().run();
        break;
      case 'blockquote':
        chain.toggleBlockquote().run();
        break;
      case 'undo':
        chain.undo().run();
        break;
      case 'redo':
        chain.redo().run();
        break;
      case 'clear':
        chain.unsetAllMarks().clearNodes().run();
        break;
    }
    this.revision.update((current) => current + 1);
  }

  ngOnDestroy(): void {
    this.editor?.destroy();
  }
}
