
```jsx

initialState = {selectedValue: 'a'};

<div>
    <Radio 
        onChange={()=>setState({selectedValue:'a'})} 
        checked={state.selectedValue === 'a'} 
        name="radio-button-demo"
        label="Opção 1"
    />
    {"\u00a0","\u00a0","\u00a0"}
    <Radio 
        onChange={()=>setState({selectedValue:'b'})} 
        checked={state.selectedValue === 'b'} 
        name="radio-button-demo"
        label="Opção 2"
    />
</div>  
        
```