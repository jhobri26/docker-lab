import { Injectable } from '@angular/core';
import DOMPurify from 'dompurify';
import { marked } from 'marked';

const ALLOWED_TAGS = [
  'p',
  'div',
  'strong',
  'b',
  'em',
  'i',
  'u',
  's',
  'del',
  'mark',
  'br',
  'h1',
  'h2',
  'h3',
  'h4',
  'ul',
  'ol',
  'li',
  'blockquote',
  'pre',
  'code',
  'hr',
  'span',
  'a',
  'table',
  'thead',
  'tbody',
  'tr',
  'th',
  'td',
];

@Injectable({ providedIn: 'root' })
export class EvidenceContentService {
  toHtml(value: string, format: 'markdown' | 'html' = 'markdown'): string {
    const html =
      format === 'html' ? value : marked.parse(value, { async: false, gfm: true, breaks: true });
    return DOMPurify.sanitize(html, {
      ALLOWED_TAGS,
      ALLOWED_ATTR: ['href', 'title', 'colspan', 'rowspan', 'start'],
      ALLOW_DATA_ATTR: false,
      ALLOW_ARIA_ATTR: false,
    });
  }

  looksLikeMarkdown(value: string): boolean {
    return /(^#{1,3}\s|^\s*[-*]\s|```|\*\*[^*\n]+\*\*)/m.test(value);
  }
}
