import React, { useEffect, useState } from 'react'

function FetchData(){
    const [records, setRecords] = useState([])

    useEffect(()=>{
        fetch('https://jsonplaceholder.typicode.com/posts')
        .then(response => response.json())
        .then(data => setRecords(data))
        .catch(err => console.log(err))
    }, [])
    return (
        <div>
            <ul>
                {records.map((list,index)=>(
                    <li key={index}> {list.id} | {list.title} <br /> <p>{list.body}</p> </li>
                    
                ))}
            </ul>
        </div>
    )
}

export default FetchData

