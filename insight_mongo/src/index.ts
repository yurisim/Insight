import express from 'express';
import { json } from 'body-parser';
import { todoRouter } from './routes/todo'

const app = express()
app.use(json())
app.use(todoRouter)

app.listen(3000, () => {
    console.log('server is listening on port 3000')
})