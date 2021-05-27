import CardContent from '@material-ui/core/CardContent';
import {DataGrid, GridToolbarContainer, GridToolbarExport} from '@material-ui/data-grid';
import '@material-ui/icons';
import { Breadcrumbs, Link, Tabs, Tab, Paper, Card, InputAdornment, FormControl,Input} from '@material-ui/core';
import "react-table/react-table.css";
import { SearchRounded } from '@material-ui/icons';


//global variable used to determine if row is seleceted
let check= false;

//Creates export function, disabled by default, and enables it when row is selected
function CustomToolbar() {
    if (check === true){
        return (
            <GridToolbarContainer>
                <GridToolbarExport disabled = {false}/>
            </GridToolbarContainer>
            )
    }

    else {
        return (
            <GridToolbarContainer>
                <GridToolbarExport disabled = {true}/>
            </GridToolbarContainer>
            )
    }
}

// export function IsChecked () {
//     if (<Checkbox checked = {true}/>) {
//         check = true;
//     } 
//     else {
//         check = false;
//     }
// }


function TBA()
{
    // const [checked, setChecked] = useState(true);

    // const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {setChecked(event.target.checked)}
    return(
        <div>
            <Paper elevation={3}>
                <Card>
                    <CardContent>
                        <Breadcrumbs separator='>>' aria-label='TBA'>
                            <Link>
                            UTM
                            </Link>
                            <Link>
                            TBA
                            </Link>
                        </Breadcrumbs>
                        <Tabs
                           
                            indicatorColor='primary'
                            textColor='primary'
                            
                            aria-label='UTM Tabs'>
                                <Tab label='All'/>
                                <Tab label='Overdue'/>
                                <Tab label='Upcoming'/>
                                <Tab label='Safe'/>
                        </Tabs>
                        <Card>
                            <div id='TopBox'>
                                <FormControl id='Test'>
                                    
                                    <Input
                                    className="Searchbar"
                                    
                                    
                                    startAdornment={
                                        <InputAdornment position="start">
                                            <SearchRounded/>
                                        </InputAdornment>
                                    }/>
                                    
                                </FormControl>
                           </div> 
                        <div style={{ height: 450}}>    
                            <DataGrid

                                //Checks to see if rows are selected, and sets check variable accordingly
                                onSelectionModelChange={(itm) => {
                                    //console.log(itm); Logs the size of the array to the console
                                    if(itm.selectionModel.length === 0)
                                    {
                                        check = false;
                                    }
                                    else{
                                        check= true;
                                    }
                                }}
                                //Adds toolbar to page
                                components={{
                                    Toolbar: CustomToolbar,
                                }}
                                //enables checkbox selection
                                checkboxSelection={true}
                                
                                columns={[
                                { field: 'Name', flex: 1 }, 
                                { field: 'Status', flex: 1}, 
                                { field: 'Tasks Completed', flex: 1 }, 
                                { field: 'Date of Last Task Completed', flex: 1 },
                                { field: 'Workcenter', flex: 1 }]}
                                rows={
                                    [
                                    {
                                    id: 1,
                                    Name: 'Withnell, Alexander',
                                    Status: 'Safe',
                                    TasksCompleted: 'Complete',
                                    DateOfLastTask: '11/07/17',
                                    Workcenter: 'DOUP',
                                },
                                {
                                    id: 2,
                                    Name: 'Myers, Ryan',
                                    Status: 'Overdue',
                                    TasksCompleted: 'Complete',
                                    DateOfLastTask: '11/07/17',
                                    Workcenter: 'DOK',
                                },
                                {
                                    id: 3,
                                    Name: 'Klinker, Michael',
                                    Status: 'Upcoming',
                                    TasksCompleted: 'Complete',
                                    DateOfLastTask: '11/07/17',
                                    Workcenter: 'DOM',
                                },
                             ]}
                                />
                        </div>
                        </Card>
                    </CardContent>
                </Card>
                </Paper>
        </div>
    )
}


export default TBA;