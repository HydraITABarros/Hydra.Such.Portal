```jsx
<div>
    <DatePickerInput staticRanges={
        [
            {label: "", ranges:
                [
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo mês"},
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo trimestre"},
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo ano"}
                ]
            },{label: "Ultimos meses", ranges:
                [
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo mês"},
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo trimestre"},
                    {from:new Date(2019, 1,1), to: new Date(2019,2,1), label:"Ultimo ano"}
                ]
            }
        ]
    }></DatePickerInput>
</div>
```