//import CardActions from '@material-ui/core/CardActions';
import CardContent from '@material-ui/core/CardContent';
import { DataGrid, GridCellParams } from '@material-ui/data-grid';
import '@material-ui/icons';
import { Breadcrumbs, Link, Tabs, Tab, Chip, Paper, Card, TextField, InputAdornment, FormControl, Input, Button } from '@material-ui/core';
//import { blue } from '@material-ui/core/colors';
import React, { useState } from 'react';
import { SearchRounded } from '@material-ui/icons';
//import { spacing } from '@material-ui/system';
import axios from 'axios';
//import { resolveProjectReferencePath } from 'typescript';



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


let rows: CreatePersonDTO[] = [];

export async function getTest()
{
     
    await axios({
        method: 'get',
        url: 'http://localhost:5000/person/getAll'
    })
       .then(res => {
           rows = res.data;
       console.log(rows[0].name);
        
        return rows;
    });
}

function Skillsoft() {
    const [checked, setChecked] = useState(true);
    let result: any;
    /*axios({
        method: 'get',
        url: 'http://localhost:5000/person/getAll'
    })
       .then(res => {
           rows  = res.data;
           result = rows[0];
           console.log(rows[0].name)
    });

console.log(result);*/

    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) => { setChecked(event.target.checked) }
    return (
        <div>
            <Paper elevation={3}>
                <Card>
                    <CardContent>
                        <Breadcrumbs separator='>>' aria-label='Skillsoft'>
                            <Link>
                                UTM
                            </Link>
                            <Link>
                                Skillsoft
                            </Link>
                        </Breadcrumbs>
                        <Tabs

                            indicatorColor='primary'
                            textColor='primary'

                            aria-label='UTM Tabs'>
                            <Tab label='All' />
                            <Tab label='Overdue' />
                            <Tab label='Upcoming' />
                            <Tab label='Safe' />
                        </Tabs>
                        <Card>
                            <div id='TopBox'>
                                <FormControl id='Test'>

                                    <Input
                                        className="Searchbar"


                                        startAdornment={
                                            <InputAdornment position="start">
                                                <SearchRounded />
                                            </InputAdornment>
                                        } />

                                </FormControl>
                                <Button id='CCButton' variant='contained' onClick={() => {
                                    let result: CreatePersonDTO[] = []
                                    getTest()
                                    .then( res => {
                                        result = res;
                                    });
                                    console.log(result);
                                    setTimeout(() => {
                                        console.log(result)
                                    });
                                }} color='primary'>Copy To Clipboard</Button>
                            </div>
                            <div style={{ height: 450 }}>
                                <DataGrid
                                
                                    checkboxSelection={true}
                                    columns={[
                                        { field: 'Name', flex: 1 },
                                        { field: 'Due Date', flex: 1 },
                                        { field: 'Status', flex: 1 },
                                        { field: 'Workcenter', flex: 1 }]}

                                    rows={rows}
                                />
                            </div>
                        </Card>
                    </CardContent>
                </Card>

            </Paper>
        </div>
    )
}


export default Skillsoft;