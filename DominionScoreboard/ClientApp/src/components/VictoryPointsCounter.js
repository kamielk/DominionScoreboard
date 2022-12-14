import React, { Component } from 'react';
import {Form} from "reactstrap";

export class VictoryPointCounter extends Component {
    static displayName = VictoryPointCounter.name;
    
    constructor(props) {
        super(props);
        this.state = { currentCount: 0, victoryCards: { estates: 0, duchies: 0, provinces: 0}};
        this.handleChange = this.handleChange.bind(this);
    }

    handleChange(event) {        
        this.setState(prevState => {
            let state = { ...prevState}
            
            // set the number of victory cards
            state.victoryCards[event.target.name] = event.target.value
            
            state.currentCount = state.victoryCards.estates * 1 + (state.victoryCards.duchies * 3) + (state.victoryCards.provinces * 8);
            
            return state;
        });
    }

    render() {
        return (
            <div>
                <h1>{this.displayName}</h1>

                <p>This counts your victorypoints</p>

                <p aria-live="polite">Current count: <strong>{this.state.currentCount}</strong></p>

                <form>
                    <div className="form-floating">
                        <input className="form-control" aria-required="true" type="number" min={0} max={12}
                               id="Input_Estates" name="estates"
                               value={this.state.estates} onChange={this.handleChange}/>
                        <label className="form-label" htmlFor="Counter_Estates">Estates</label>
                    </div>

                    <div className="form-floating">
                        <input className="form-control" aria-required="true" type="number" min={0} max={12}
                               id="Counter_Duchies" name="duchies"
                               value={this.state.duchies} onChange={this.handleChange}/>
                        <label className="form-label" htmlFor="Counter_Duchies">Duchies</label>
                    </div>

                    <div className="form-floating">
                        <input className="form-control" aria-required="true" type="number" min={0} max={12}
                               id="Counter_Provinces" name="provinces"
                               value={this.state.provinces} onChange={this.handleChange}/>
                        <label className="form-label" htmlFor="Counter_Provinces">Provinces</label>
                    </div>
                
                </form>
            </div>
        );
    }
}
