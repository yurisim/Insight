import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import {DataGrid, GridCellParams} from '@material-ui/data-grid';
import '@material-ui/icons';
import { Breadcrumbs, Link, Tabs, Tab, Chip, Paper, Card, TextField, InputAdornment, FormControl,Input, Button} from '@material-ui/core';
import { blue } from '@material-ui/core/colors';
import React, { useRef, useState, Component } from 'react';
import "react-table/react-table.css";
import data from "./data.json"
import { SearchRounded } from '@material-ui/icons';
import {spacing} from '@material-ui/system';

const ReactTableCopyWrapper = require ('react-table-copy-wrapper');

function TBA()
{
    const [checked, setChecked] = useState(true);

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => {setChecked(event.target.checked)}
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
                                <Button 
                                    onClick={() =>  navigator.clipboard.writeText('Copy this text to clipboard')}
                                >
                                    Copy
                                </Button>
                                {/* <Button id='CCButton' variant='contained' color='primary'>Copy To Clipboard</Button> */}
                           </div> 
                        <div style={{ height: 450}}>    
                            <DataGrid 
                                
                                checkboxSelection={true}
                                columns={[
                                { field: 'Name', flex: 1 }, 
                                { field: 'Status (Chip Column)', flex: 1}, 
                                { field: 'Tasks Completed', flex: 1 }, 
                                { field: 'Workcenter', flex: 1 }]}
                                rows={
                                    [
                                    {
                                    id: 1,
                                    Name: 'Withnell, Alexander',
                                    Status: 'Add Chip',
                                    TasksCompleted: 'Complete',
                                    Workcenter: 'DOUP',
                                }, ]}
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