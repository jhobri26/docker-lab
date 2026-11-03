import { HttpClient, HttpErrorResponse } from '@angular/common/http';
import { inject, Injectable } from '@angular/core';
import { firstValueFrom, Observable } from 'rxjs';
import {
  ExamData,
  Evidence,
  EvidenceUpdate,
  ProfileUpdate,
  ProgressUpdate,
  RequirementProgress,
  ServiceStatus,
  StudentProfile,
} from '../models/exam.models';

@Injectable({ providedIn: 'root' })
export class ExamApiService {
  private readonly http = inject(HttpClient);
  private readonly baseUrl = '/api';

  getExam(): Promise<ExamData> {
    return this.send(this.http.get<ExamData>(`${this.baseUrl}/exam`));
  }

  getServices(): Promise<ServiceStatus> {
    return this.send(this.http.get<ServiceStatus>(`${this.baseUrl}/services`));
  }

  updateProgress(id: number, progress: ProgressUpdate): Promise<RequirementProgress> {
    return this.send(
      this.http.put<RequirementProgress>(`${this.baseUrl}/requirements/${id}`, progress),
    );
  }

  updateProfile(profile: ProfileUpdate): Promise<StudentProfile> {
    return this.send(this.http.put<StudentProfile>(`${this.baseUrl}/profile`, profile));
  }

  uploadEvidence(
    id: number,
    file: File,
    title: string,
    description: string,
    descriptionFormat: 'markdown' | 'html' = 'html',
  ): Promise<{ id: number }> {
    const form = new FormData();
    form.append('file', file);
    form.append('title', title);
    form.append('description', description);
    form.append('descriptionFormat', descriptionFormat);
    return this.send(
      this.http.post<{ id: number }>(`${this.baseUrl}/requirements/${id}/evidence`, form),
    );
  }

  deleteEvidence(id: number): Promise<void> {
    return this.send(this.http.delete<void>(`${this.baseUrl}/evidence/${id}`));
  }

  getEvidence(id: number): Promise<Evidence> {
    return this.send(this.http.get<Evidence>(`${this.baseUrl}/evidence/${id}`));
  }

  updateEvidence(id: number, evidence: EvidenceUpdate): Promise<Evidence> {
    return this.send(this.http.put<Evidence>(`${this.baseUrl}/evidence/${id}`, evidence));
  }

  async downloadFinalReport(): Promise<{ blob: Blob; fileName: string }> {
    const response = await this.send(this.http.get(`${this.baseUrl}/reports/final.pdf`, {
      observe: 'response', responseType: 'blob',
    }));
    if (!response.body?.size) throw new Error('El servidor no devolvió el informe. Inténtalo nuevamente.');
    const disposition = response.headers.get('Content-Disposition') ?? '';
    const name = /filename="?([^";]+)"?/i.exec(disposition)?.[1];
    return { blob: response.body, fileName: name ?? 'informe-docker.pdf' };
  }

  private async send<T>(request: Observable<T>): Promise<T> {
    try {
      return await firstValueFrom(request);
    } catch (error) {
      const response = error as HttpErrorResponse;
      let problem = response.error;
      if (problem instanceof Blob) {
        try { problem = JSON.parse(await problem.text()); }
        catch { problem = null; }
      }
      const validation = problem?.errors ? Object.values(problem.errors).flat().join(' ') : '';
      throw new Error(
        problem?.message ||
          validation ||
          problem?.detail ||
          problem?.title ||
          'No se pudo completar la operación. Revisa la conexión con los servicios.',
      );
    }
  }
}
