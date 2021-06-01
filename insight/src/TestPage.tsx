import React, { useState } from 'react';
import {Button} from '@material-ui/core'

function TestPage(){
    const [checked, setChecked] = useState(true);
    let result: any;
    const handleChange = (event: React.ChangeEvent<HTMLInputElement>) =>{ setChecked(event.target.checked) }
    return (
        <div>
            <Button id='CCButton' variant='contained' onClick={() => {}} color='primary'>Copy To Clipboard</Button>
        </div>
    )
}

export default TestPage;