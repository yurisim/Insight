import { GridColumns } from '@material-ui/x-grid'

export const columns: GridColumns = [
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