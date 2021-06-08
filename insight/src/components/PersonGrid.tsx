import { DataGrid, GridToolbarContainer, GridToolbarExport, 
  GridColumnsToolbarButton, GridFilterToolbarButton, 
  GridColDef } from '@material-ui/data-grid';
import SearchIcon from '@material-ui/icons/Search'
import { TextField, Button, InputAdornment } from '@material-ui/core';
import { Component } from 'react';
import axios from 'axios';
import { getAllJSDocTags } from 'typescript';

/* Handles the Export Button Toggle */
let nothingSelected = true;

// /* Handles the Tab Filters */
// let tab = "";

// /* Sets filter for 'Safe' Tab */
// const safeFilter: GridFilterModel = {
//   items: [
//     {columnField: 'status', operatorValue: 'contains', value: 'Safe'}
//   ],
//   linkOperator: GridLinkOperator.Or,
// };


// /* Sets filter for 'Upcoming' Tab */
// const upcomingFilter: GridFilterModel = {
//   items: [
//     {columnField: 'status', operatorValue: 'contains', value: 'Upcoming'}
//   ],
//   linkOperator: GridLinkOperator.Or,
// };


// /* Sets filter for 'Overdue' Tab */
// const overdueFilter: GridFilterModel = {
//   items: [
//      {columnField: 'status', operatorValue: 'contains', value: 'Overdue'}
//   ],
//   linkOperator: GridLinkOperator.Or,
// };

// const tabFilter: GridFilterModel = {
//   items: [
//     {columnField: 'status', operatorValue: 'contains', value: tab}
//   ],
//   linkOperator: GridLinkOperator.Or,
// };


/* This is the tool bar that renders above the grid */
function PersonGridToolBar() {
  return (
    <GridToolbarContainer>
      
      {/* Filter Columns */}
      <GridColumnsToolbarButton />

      {/* Filter by search  */}
      <GridFilterToolbarButton />

      {/* The export button is disabled depending on what is toggled */}
      <GridToolbarExport disabled={nothingSelected} />
    </GridToolbarContainer>
  );
}

/* Whatever you stick into here will become the properties of the UIElement */ 

/* Whatever you stick into here will become the properties of the UIElement*/ 
interface IPersonGridProps {
  daColumns: GridColDef[];
}

/* Whatever you stick into here will become the state of the UIElement*/ 
interface IPersonGridState {
  rows: [],
  search_value: string,
}

class PersonGrid extends Component<IPersonGridProps, IPersonGridState> {
  constructor(props){
    super(props);

    this.searchClick = this.searchClick.bind(this);
    //this.GetData = this.GetData.bind(this);
    this.handleChange = this.handleChange.bind(this);
    
    this.state = {
      rows: [],
      search_value: ''
    }
  }

  handleChange(e: React.ChangeEvent<HTMLInputElement>) {
    console.log(e.target.value)
    this.setState({search_value: e.target.value});
  }
  
  public PersonGridToolBar() {
    return (
      <GridToolbarContainer>
        {/* Filter Columns */}
        <GridColumnsToolbarButton />
  
        {/* Filter by search  */}
        {/*<GridFilterToolbarButton />*/}
  
        {/* The export button is disabled depending on what is toggled */}
        <GridToolbarExport disabled={nothingSelected} variant="contained" />
      </GridToolbarContainer>
    );
  }

  searchClick(){
    this.GetData('http://localhost:5000/person/search/'+ this.state.search_value);
  }

  GetData( inputUrl:string) {
    axios({
      method: 'get',
      url: inputUrl
    })
    .then(res => {
      let rows = res.data;
      rows.forEach(element => {
        element.id = element._id
      });
      this.setState({ rows : res.data });
    });
  }

  componentDidMount(){
    this.GetData('http://localhost:5000/person/getAll')
  }

  render() {

    return (
      
      <div style={{ marginTop: 10, width: 'auto' }}>
        
        <div style={{ height: 500, width: 'auto' }}>
          {/* shrink:true makes it so the text is aligned with the top of the search box*/ }
        <TextField id="searchBox" label="SearchBox" variant="outlined" size="small" InputLabelProps={{shrink: true}} onChange={ this.handleChange }
              InputProps=
              {{ endAdornment : (
                <InputAdornment position="end">
                  <SearchIcon color="primary" />
                </InputAdornment>
              )
              }}
            />
            <Button id="submitButton" variant="outlined" color="primary" onClick={this.searchClick} > Search </Button>
          {/* Stick daRows and daColumns as a property of this react component in the constructor */}
          <DataGrid rows={this.state.rows} columns={this.props.daColumns} pageSize={8} density="compact"

            /* Puts a checkbox in the first column of the table */
            checkboxSelection

            // pagination

            // rowsPerPageOptions={[10,20,30,40]}

            /* Whenever someone selects or deselects a checkbox, count the length of their selection.  */
            onSelectionModelChange={(newSelection) => {
              /* if at least one is selected, change the state of the export button */
              nothingSelected = newSelection.selectionModel.length <= 0;
            }}

            components={{
              Toolbar: this.PersonGridToolBar,
            }}
          />
        </div>
      </div>
    )
  }
}

export default PersonGrid