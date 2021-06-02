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

/* This is the tool bar that renders above the grid */
function PersonGridToolBar() {
  return (
    <GridToolbarContainer>
          {/* shrink:true makes it so the text is aligned with the top of the search box*/ }
          <TextField label="Search" variant="outlined" InputLabelProps={{shrink: true}} 
            InputProps=
            {{ endAdornment : (
              <InputAdornment position="end">
                <SearchIcon color="primary" />
              </InputAdornment>
            )
            }}
          />
          <Button variant="outlined" color="primary" > Search </Button>

      {/* Filter Columns */}
      <GridColumnsToolbarButton />

      {/* Filter by search  */}
      <GridFilterToolbarButton />

      {/* The export button is disabled depending on what is toggled */}
      <GridToolbarExport disabled={nothingSelected} variant="contained" />
    </GridToolbarContainer>
  );
}

/* Whatever you stick into here will become the properties of the UIElement*/ 
interface IPersonGridProps {
  daColumns: GridColDef[];
}

class PersonGrid extends Component<IPersonGridProps> {
  state = {
    rows: []
  }

  searchInput:string = ''

  public PersonGridToolBar() {
    return (
      <GridToolbarContainer>
            {/* shrink:true makes it so the text is aligned with the top of the search box*/ }
            <TextField id="searchBox" label="SearchBox" variant="outlined" InputLabelProps={{shrink: true}} 
              InputProps=
              {{ endAdornment : (
                <InputAdornment position="end">
                  <SearchIcon color="primary" />
                </InputAdornment>
              )
              }}
            />
            <Button id="submitButton" variant="outlined" color="primary"> Search </Button>
  
        {/* Filter Columns */}
        <GridColumnsToolbarButton />
  
        {/* Filter by search  */}
        <GridFilterToolbarButton />
  
        {/* The export button is disabled depending on what is toggled */}
        <GridToolbarExport disabled={nothingSelected} variant="contained" />
      </GridToolbarContainer>
    );
  }


  public GetData( inputUrl:string) {

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
      <div>
        <div style={{ height: 500, width: 'auto' }}>
          {/* Stick daRows and daColumns as a property of this react component in the constructor */}
          <DataGrid rows={this.state.rows} columns={this.props.daColumns} pageSize={8} density="compact"

            /* Puts a checkbox in the first column of the table */
            checkboxSelection

            /* Whenever someone selects or deselects a checkbox, count the length of their selection.  */
            onSelectionModelChange={(newSelection) => {
              /* if at least one is selected, change the state of the export button */
              nothingSelected = (newSelection.selectionModel.length > 0) ? false : true;
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