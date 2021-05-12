// import { Prop, Schema, SchemaFactory } from '@nestjs/mongoose';
import { Document } from 'mongoose';
import * as mongoose from 'mongoose';

// export type PersonDocument = Person & Document;

// Need to declare this before it's used
export enum PersonStatus {
  CURRENT = 'Current',
  UPCOMING = 'Upcoming',
  OVERDUE = 'Overdue',
}

export const PersonSchema = new mongoose.Schema({
  _id: Number, //DoDID
  name: String,
  _AFSCID: Number,//refrences foreign key
  workCenter: String,
  timeOnStation: Date,
  // this can't be an enum, sorry. Valid schema types are below
  // see https://mongoosejs.com/docs/guide.html#definition
  status: String,
  dueDate: Date,
  comments: String,
});

export interface Person extends Document {
  _id: number;
  name: string;
  _AFSCID: number;//refrences foreign key
  workCenter: string;
  timeOnStation: Date;
  status: PersonStatus;
  dueDate: Date;
  comments: string;
}

// @Schema()
// export class Person {
//   @Prop()
//   name: string;

//   @Prop()
//   workcenter: string; //enum?

//   @Prop()
//   status: PersonStatus;

//   @Prop()
//   duedate: Date;
// }

// export enum PersonStatus {
//   CURRENT = 'CURRENT',
//   UPCOMING = 'UPCOMING',
//   OVERDUE = 'OVERDUE',
// }

// export const PersonSchema = SchemaFactory.createForClass(Person);
