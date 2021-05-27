import { DataGrid, GridToolbarContainer, GridToolbarExport, 
  GridColumnsToolbarButton, GridFilterToolbarButton, 
  GridColDef } from '@material-ui/data-grid';
import { Component } from 'react';

/* Handles the Export Button Toggle */
let nothingSelected = true;

/* This is the tool bar that renders above the grid */
function PersonGridToolBar() {
  return (
    <GridToolbarContainer>

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
  daRows: any;
  daColumns: GridColDef[];
}

class PersonGrid extends Component<IPersonGridProps> {

  render() {
    return (
      <div>
        <div style={{ height: 500, width: 'auto' }}>

          {/* Stick daRows and daColumns as a property of this react component in the constructor */}
          <DataGrid rows={this.props.daRows} columns={this.props.daColumns} pageSize={8} density="compact"

            /* Puts a checkbox in the first column of the table */
            checkboxSelection

            /* Whenever someone selects or deselects a checkbox, count the length of their selection.  */
            onSelectionModelChange={(newSelection) => {
              /* if at least one is selected, change the state of the export button */
              nothingSelected = (newSelection.selectionModel.length > 0) ? false : true;
            }}

            components={{
              Toolbar: PersonGridToolBar,
            }}
          />
        </div>
      </div>
    )
  }
}

export default PersonGrid