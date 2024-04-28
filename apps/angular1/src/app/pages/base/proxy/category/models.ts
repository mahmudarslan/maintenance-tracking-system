export class CategoryDto {
  id?: string;
  parentId?: string;
  name: string;
  isDeleted: boolean;
}

export class CreateUpdateCategoryDto {
  id: string;
  parentId?: string;
  name: string;
  isDeleted: boolean;
}
