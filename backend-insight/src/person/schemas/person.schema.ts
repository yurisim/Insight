import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';

export type PersonDocument = Person & Document;

@Schema()
export class Person {
    @Prop()
    name : string;

    @Prop()
    workcenter : string; //enum?

    @Prop()
    status : PersonStatus;

    @Prop()
    duedate : Date;

}

export enum PersonStatus {
    CURRENT = 'CURRENT',
    UPCOMING = 'UPCOMING',
    OVERDUE = 'OVERDUE',
  }

export const PersonSchema = SchemaFactory.createForClass(Person);