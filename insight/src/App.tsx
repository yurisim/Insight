import './App.css';
import PersonGrid from './components/PersonGrid'
import StatusTabs from './components/StatusTabs'
import CssBaseline from '@material-ui/core/CssBaseline'
import {columns} from './junkdata';
import { Component } from "react";
import axios from "axios";
import { GridRowsProp } from "@material-ui/x-grid";

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

/**
 * Represents the status tabs' possible values
 */
export enum enumStatusView { OVERDUE = "OVERDUE", SAFE = "SAFE", UPCOMING = "UPCOMING", ALL = "ALL", }

/**
 * App State
 */
interface IAppState {
  selectedStatus: enumStatusView;
  persons: GridRowsProp;
}

class App extends Component<{}, IAppState> {
  constructor(props) {
    super(props);
    this.state = {
      //This sets default status tab
      selectedStatus: enumStatusView.ALL,
      persons: [],
    };
    this.getData("http://localhost:5000/person/getAll");
  }

  /**
   * Async data retrival from backend
   * @param inputUrl
   */
  getData(inputUrl: string) {
    axios({
      method: "get",
      url: inputUrl,
    }).then((res) => {
      let persons = res.data;

      persons.forEach((element) => {
        element.id = element._id;
      });
      this.setState({ persons: persons });
    });
  }

  /**
   * called when selected tab changes, stores value in state
   * @param status
   */
  changeStatusTab(status: enumStatusView) {
    this.setState({ selectedStatus: status });
  }

  render() {
    return (
      <div className="App">
        <CssBaseline />
        <StatusTabs
          selectedStatus={this.state.selectedStatus}
          changeStatusTab={(selectedStatus) => this.changeStatusTab(selectedStatus) }
        />
        <PersonGrid
          columns={columns}
          persons={this.state.persons}
          selectedStatus={this.state.selectedStatus}
        />
      </div>
    );
  }
}

export default App;