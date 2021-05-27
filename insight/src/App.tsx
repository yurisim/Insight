<<<<<<< HEAD
import Skillsoft from './Skillsoft';
import TestPage from './TestPage'
=======
>>>>>>> 219b0f8dd1f3efe8fb0ad4dce2d2c4ae7fbb24b5
import './App.css';
import PersonGrid from './components/PersonGrid'
import CssBaseline from '@material-ui/core/CssBaseline'

import {rows} from './junkdata';
import {columns} from './junkdata';


function App() {
  return (
    <div className="App">
      <CssBaseline/>
      {/* <Skillsoft/> */}
      <PersonGrid daRows={rows} daColumns={columns}/>
    </div>
  );
}

export default App;