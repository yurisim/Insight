import express, { Request, Response} from 'express'

const router = express.Router()

//TODO: understand what square brackets mean
router.get('/api/todo', [], (req: Request, res: Response) => {
    return res.send('the todo')
})

//TODO: understand what square brackets mean
router.post('/api/todo', [], (req: Request, res: Response) => {
    return res.send('new todo created')
})

export { router as todoRouter }