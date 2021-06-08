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



/* Whatever you stick into here will become the properties of the UIElement */
interface TabPanelProps {
    children?: React.ReactNode;
    dir?: string;
    index: any;
    value: any;
}

/* Needed to create tabs, no clue as to what it does exactly */
function TabPanel(props: TabPanelProps) {
    const { children, value, index, ...other} = props;

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

/* creates tabs */
export default class StatusTabs extends React.Component<{ }, { value: string }> {

    constructor(props) {
        super(props);
        this.state = {
            value: 'None',
        };
    }

    handleChange = (_, value) => {
        this.setState({
            value,
        });
    };

    render() {
        return (
            <div>
                <Tabs
                    value={this.state.value}
                    onChange={this.handleChange}
                    indicatorColor="primary"
                >
                    <Tab icon={<PeopleAltIcon />} style={{ color: indigo[500]}} label="All" {...allyProps(0)} value="All" />
                    <Tab icon={<CheckCircleIcon />} style={{ color: green[500]}} label="Safe" {...allyProps(1)} value="Safe" />
                    <Tab icon={<WarningIcon />} style={{ color: orange[400]}} label="Upcoming" {...allyProps(2)} value="Upcoming" />
                    <Tab icon={<ErrorIcon />} style={{ color: red[500]}} label="Overdue" {...allyProps(3)} value="Overdue" />
                </Tabs>
                <TabPanel value={"All"} index={0}>
                </TabPanel>
                <TabPanel value={"Safe"} index={1}>
                </TabPanel>
                <TabPanel value={"Upcoming"} index={2}>   
                </TabPanel>
                <TabPanel value={"Overdue"} index={3}>
                </TabPanel>
            </div>
        )
    }
}