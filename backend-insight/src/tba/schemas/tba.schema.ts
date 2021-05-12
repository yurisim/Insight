import { Document } from 'mongoose';
import * as mongoose from 'mongoose';

export const TbaSchema = new mongoose.Schema({
    _id: Number,
    tasksCompleted: Number,
    dateLastUpdate: Date,
});

export  interface Person extends Document {
    _id: number;
    tasksCompleted
};