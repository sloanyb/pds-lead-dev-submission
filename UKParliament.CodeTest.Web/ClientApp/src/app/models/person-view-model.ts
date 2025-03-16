export interface PersonGetDepartmentViewModel {
  id: number;
  departmentName: string;
}

export interface PersonGetViewModel {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  department: PersonGetDepartmentViewModel;
}

export interface PersonUpdateViewModel {
  id: number;
  firstName: string;
  lastName: string;
  dateOfBirth: string;
  departmentId: number;
}
