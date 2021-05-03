import { PersonStatus } from '../schemas/person.schema';

export class CreatePersonDTO {
  name: string;
  workCenter: string;
  status: PersonStatus;
  dueDate: Date;
}
