import { PersonStatus } from '../schemas/person.schema'

export class CreatePersonDto {
    name : string;
    workcenter : string;
    status : PersonStatus;
    duedate : Date;
  }