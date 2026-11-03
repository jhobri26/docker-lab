export type RequirementStatus = 'pendiente' | 'en-progreso' | 'listo';
export type PortalView = 'overview' | 'case' | 'architecture' | 'resources';

export const STATUS_LABELS: Record<RequirementStatus, string> = {
  pendiente: 'Pendiente',
  'en-progreso': 'En progreso',
  listo: 'Listo para revisar',
};

export interface StudentProfile {
  id: number;
  student: string;
  repositoryUrl: string;
}

export interface RequirementProgress {
  id: number;
  status: RequirementStatus;
  notes: string;
  updatedAt: string;
}

export interface Evidence {
  id: number;
  title: string;
  description: string;
  descriptionFormat?: 'markdown' | 'html';
  size: number;
  uploadedAt: string;
  url: string;
}

export interface EvidenceUpdate {
  title: string;
  description: string;
  descriptionFormat: 'markdown' | 'html';
}

export interface Requirement {
  id: number;
  title: string;
  summary: string;
  requirement: string;
  points: number;
  checklist: string[];
  commands: { label: string; code: string }[];
  evidenceGuide: string[];
  note: string;
  progress: RequirementProgress;
  evidence: Evidence[];
}

export interface ExamData {
  profile: StudentProfile;
  requirements: Requirement[];
}

export interface ServiceStatus {
  api: boolean;
  database: boolean;
  redis: boolean;
  visits: number | null;
  version: string;
}

export interface ProgressUpdate {
  status: RequirementStatus;
  notes: string;
}

export type ProfileUpdate = Pick<StudentProfile, 'student' | 'repositoryUrl'>;
