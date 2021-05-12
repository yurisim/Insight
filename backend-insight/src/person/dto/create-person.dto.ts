import { PersonStatus } from '../schemas/person.schema';

export class CreatePersonDTO {
  _id: number;//is DoDID
  name: string;
  _AFSCID: number;//refrences foreign key
  workCenter: string;
  timeOnStation: Date;
  status: PersonStatus;
  dueDate: Date;
  comments: string;
}
