import './App.css';
import PersonGrid from './components/PersonGrid'
import StatusTabs from './components/StatusTabs'
import CssBaseline from '@material-ui/core/CssBaseline'
import {columns} from './junkdata';

export interface CreatePersonDTO {
  name: string;
  dodid: number;
  afscid: number;
  workCenter: string;
  timeOnStation: Date
  status: string;
  dueDate: Date;
  comments: string;
}

function App(){
  return (
    <div className="App">
      <CssBaseline/>
      <StatusTabs />
      <PersonGrid daColumns={columns}/>
    </div>
  );
}

export default App;