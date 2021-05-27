import { GridColDef } from '@material-ui/data-grid';

export const columns: GridColDef[] = [
    { field: 'id', headerName: 'ID', hide: true },
    { field: 'dodid', headerName: 'DoDID', width: 125 },
    { field: 'lastName', headerName: 'Last Name', width: 150 },
    { field: 'firstName', headerName: 'First Name', width: 150 },
    { field: 'afscid', headerName: 'AFSC', width: 125},
    { field: 'workCenter', headerName: 'Work Center', width: 170 },
    { field: 'timeOnStation', headerName: 'TOS', width: 110 },
    { field: 'status', headerName: 'Status', width:120 },
    { field: 'dueDate', headerName: 'Due Date', width:130 },
    { field: 'comments', headerName: 'Comments', width: 200, editable: true },
  ];

export const rows = [
    {
        id: 1,
        lastName: "Castle",
        firstName: "Grif",
        dodid: 1234567890,
        afscid: 4,
        workCenter: "DOUP",
        timeOnStation: "10/06/2018",
        status: "Safe",
        dueDate: "10/06/2019",
        comments: "awesome"
    },
    {
        id: 2,
        lastName: "Sim",
        firstName: "Yuri",
        dodid: 1234567891,
        afscid: 4,
        workCenter: "DOUP",
        timeOnStation: "07/06/2020",
        status: "Upcoming",
        dueDate: "07/06/2021",
        comments: "eh"
    },
    {
        id: 3,
        lastName: "Myers",
        firstName: "Ryan",
        dodid: 1234567892,
        afscid: 3,
        workCenter: "DOK",
        timeOnStation: "11/06/2018",
        status: "Safe",
        dueDate: "10/06/2019",
        comments: "awesome-ish"
    },
    {
        id: 4,
        lastName: "Sullivan",
        firstName: "Nathan",
        dodid: 1234567893,
        afscid: 2,
        workCenter: "DOM",
        timeOnStation: "03/06/2019",
        status: "Safe",
        dueDate: "03/06/2020",
        comments: "awesome"
    },
    {
        id: 5,
        lastName: "Withnell",
        firstName: "Alexander",
        dodid: 1234567894,
        afscid: 4,
        workCenter: "DOUP",
        timeOnStation: "10/06/2018",
        status: "Overdue",
        dueDate: "10/06/2019",
        comments: "sleepy"
    }
  ];
