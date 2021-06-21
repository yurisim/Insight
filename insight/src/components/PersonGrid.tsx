import { XGrid, GridColumns, GridRowsProp, getThemePaletteMode } from "@material-ui/x-grid";
import SearchIcon from "@material-ui/icons/Search";
import { TextField, Button, InputAdornment } from "@material-ui/core";
import { Component } from "react";
import { createMuiTheme, darken, lighten, Theme, withStyles, WithStyles } from "@material-ui/core/styles";
import { enumStatusView } from "../App";
import axios from 'axios';
import { getAllJSDocTags } from 'typescript';

/* Handles the Export Button Toggle */
let nothingSelected = true;

const defaultTheme = createMuiTheme();

const styles = (theme: Theme) => {
  const getBackgroundColor = (color) => getThemePaletteMode(theme.palette) === "dark" ? darken(color, 0.6) : lighten(color, 0.6);
  const getHoverColor = (color) => getThemePaletteMode(theme.palette) === "dark" ? darken(color, 0.5) : lighten(color, 0.5);

  return {
    root: {
      /*'& .super-app-theme--SAFE':{
          backgroundColor: getBackgroundColor(theme.palette.info.main),
          '&:hover': {
            backgroundColor: getHoverColor(theme.palette.info.main),
          },
        },*/
      "& .super-app-theme--UPCOMING": { backgroundColor: getBackgroundColor(theme.palette.warning.main),
        "&:hover": { backgroundColor: getHoverColor(theme.palette.warning.main) },
      },
      "& .super-app-theme--OVERDUE": { backgroundColor: getBackgroundColor(theme.palette.error.main),
        "&:hover": { backgroundColor: getHoverColor(theme.palette.error.main),
        },
      },
    },
  };
};
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

/**
 * PersonGrid Props
 */
interface IPersonGridProps extends WithStyles<typeof styles> {
  columns: GridColumns;
  persons: GridRowsProp;
  selectedStatus: enumStatusView;
}

/* Whatever you stick into here will become the state of the UIElement*/ 
interface IPersonGridState {
  search_textbox_value: string;
  search_key: string;
  rows: GridRowsProp;
}

class PersonGrid extends Component<IPersonGridProps, IPersonGridState> {
  constructor(props){
    super(props);

    this.searchClick = this.searchClick.bind(this);
    this.handleTabChange = this.handleTabChange.bind(this);
    this.keyPress = this.keyPress.bind(this);

    this.state = {
      search_textbox_value: "",
      search_key: "",
      rows: this.props.persons,
    };
  }

  /**
   * Tab selection change event handler
   * @param e
   */
  handleTabChange(e: React.ChangeEvent<HTMLInputElement>) {
    //console.log(e.target.value);
    this.setState({ search_textbox_value: e.target.value });
  }

  keyPress(e){
    if(e.key === "Enter"){
      this.setState({
        search_key: this.state.search_textbox_value.toLowerCase(),
      });
    }
 }

  /**
   * Search button click handler
   * @param e
   */
  searchClick(e) {
    this.setState({
      search_key: this.state.search_textbox_value.toLowerCase(),
    });
  }

  /**
   * react lifecycle method. Handles filtering/searching of data
   * @param nextProps
   * @param nextState
   * @returns
   */
  shouldComponentUpdate(nextProps, nextState) {
    //resets data to everything
    //downside is filtering/searching is done every rerender
    nextState.rows = nextProps.persons;

    //searchs data for key from textbox
    if (nextState.search_key !== "") {
      nextState.rows = nextState.rows.filter(
        //fields listed here the ones that are searched
        ({ firstName, lastName, workCenter }) =>
          [firstName, lastName, workCenter].some((field) =>
            `${field || ""}`.toLowerCase().includes(nextState.search_key)
          )
      );
    }

    //filter by status if not on ALL
    if (nextProps.selectedStatus !== enumStatusView.ALL) {
      nextState.rows = nextState.rows.filter((person) => {
        return person.status === nextProps.selectedStatus;
      });
    }
    return true;
  }

  render() {
    const { classes } = this.props;
    return (
      <div style={{ marginTop: 10, width: "auto" }}>
        <div style={{ height: 400, width: "auto" }} className={classes.root}>
          {/* shrink:true makes it so the text is aligned with the top of the search box*/}
          <div style={{ display: "flex", alignItems: "center" }}>
            <TextField
              id="searchBox"
              label="SearchBox"
              variant="outlined"
              size="small"
              InputLabelProps={{ shrink: true }}
              onChange={this.handleTabChange}
              onKeyDown={this.keyPress}
              InputProps={{
                endAdornment: (
                  <InputAdornment position="end">
                    <SearchIcon color="primary" />
                  </InputAdornment>
                ),
              }}
            />
            <Button
              id="submitButton"
              variant="outlined"
              color="primary"
              onClick={(e) => this.searchClick(e)}
            >
              {" "}
              Search{" "}
            </Button>
          </div>
          {/* Stick rows and columns as a property of this react component in the constructor */}
          <XGrid
            rows={this.state.rows}
            columns={this.props.columns}
            pageSize={8}
            density="compact"
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
              //Toolbar: this.PersonGridToolBar,
            }}
            getRowClassName={(params) =>
              `super-app-theme--${params.getValue(params.id, "status")}`
            }
          />
        </div>
      </div>
    )
  }
}

export default withStyles(styles)(PersonGrid);
