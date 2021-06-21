import React from 'react';
import { makeStyles, StylesProvider, Theme, ThemeProvider, useTheme } from '@material-ui/core/styles';
import AppBar from '@material-ui/core/AppBar';
import Tabs from '@material-ui/core/Tabs';
import Tab from '@material-ui/core/Tab';
import Typography from '@material-ui/core/Typography';
import Box from '@material-ui/core/Box';
import WarningIcon from '@material-ui/icons/Warning';
import ErrorIcon from '@material-ui/icons/Error';
import CheckCircleIcon from '@material-ui/icons/CheckCircle';
import PeopleAltIcon from '@material-ui/icons/PeopleAlt';
import { green, orange, red, indigo } from '@material-ui/core/colors';
import { enumStatusView } from "../App";


/* Whatever you stick into here will become the properties of the UIElement */
interface TabPanelProps {
    children?: React.ReactNode;
    dir?: string;
    index: any;
    value: any;
}

/* Needed to create tabs, no clue as to what it does exactly */
function TabPanel(props: TabPanelProps) {
  const { children, value, index, ...other } = props;

  return (
    <div
      role="tabpanel"
      hidden={value !== index}
      id={`full-width-tabpanel-${index}`}
      aria-labelledby={`full-width-tabpanel-${index}`}
      {...other}
    >
      {value === index && (
        <Box p={4}>
          <Typography>{children}</Typography>
        </Box>
      )}
    </div>
  );
}

/* Sets the ID of a tab based on it's location in an array */
function  allyProps(index: any) {
    return {
        id: `full-width-tabpanel-${index}`,
        'aria-labelledby':`full-width-tabpanel-${index}`
    };
}

/* Used to set the design style of the tabs */
const styles = makeStyles((theme: Theme) => ({
    root: {
        backgroundColor: theme.palette.background.paper,
    },

}));


/**
 * StatusTabs Props
 */
interface IStatusTabsProps {
  selectedStatus: enumStatusView;
  changeStatusTab: (status: enumStatusView) => void;
}

/**
 * Tabs that select which persons by status to display
 */
export default class StatusTabs extends React.Component<IStatusTabsProps> {
  render() {
    return (
      <div>
        <Tabs value={this.props.selectedStatus} onChange={ (_, value) => { this.props.changeStatusTab(value) } } indicatorColor="primary">
          <Tab icon={<PeopleAltIcon />} style={{ color: indigo[500] }} {...allyProps(0)} value={enumStatusView.ALL} /> 
          <Tab icon={<CheckCircleIcon />} style={{ color: green[500] }} label="Safe" {...allyProps(1)} value={enumStatusView.SAFE} /> 
          <Tab icon={<WarningIcon />} style={{ color: orange[400] }} label="Upcoming" {...allyProps(2)} value={enumStatusView.UPCOMING} /> 
          <Tab icon={<ErrorIcon />} style={{ color: red[500] }} label="Overdue" {...allyProps(3)} value={enumStatusView.OVERDUE} />
        </Tabs>
        <TabPanel value={enumStatusView.ALL} index={0}></TabPanel>
        <TabPanel value={enumStatusView.SAFE} index={1}></TabPanel>
        <TabPanel value={enumStatusView.UPCOMING} index={2}></TabPanel>
        <TabPanel value={enumStatusView.OVERDUE} index={3}></TabPanel>
      </div>
    );
  }
}
