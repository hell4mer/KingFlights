import React, { Component } from 'react';

export class Flights extends Component {
    static displayName = Flights.name;

    constructor(props) {
        super(props);
        this.state = { flights: [], loading: false, hasRecords: false, firstLoad:true };

        this.handleSubmit = this.handleSubmit.bind(this);
    }

    handleSubmit(e) {
        e.preventDefault();
        this.populateFlightsData();
    }

    static renderFlightsTable(flights) {
        return (
            <table className='table table-striped' aria-labelledby="tabelLabel">
                <thead>
                    <tr>
                        <th>Origin</th>
                        <th>Destination</th>
                        <th>Date</th>
                        <th>Transfers</th>
                        <th>Passengers</th>
                        <th>Price</th>
                    </tr>
                </thead>
                <tbody>
                    {flights.map(flight =>
                        <tr key={flight.id}>
                            <td>{flight.originLocationCode}</td>
                            <td>{flight.destinationLocationCode}</td>
                            <td>{new Date(flight.departureDate).toLocaleDateString('hr-HR')}</td>
                            <td>{flight.transfers}</td>
                            <td>{flight.passengers}</td>
                            <td>{flight.price + " " + flight.currencyCode}</td>
                        </tr>
                    )}
                </tbody>
            </table>
        );
    }

    render() {
        let contents = !this.state.loading ? this.state.hasRecords ? Flights.renderFlightsTable(this.state.flights)
            : !this.state.firstLoad ? <p><em>No results. Please refine your search parameters.</em></p> : <div /> : <div />;
        let loadingAreaText = this.state.loading ? <span><em> Loading...</em></span> : <div />;

        return (
            <div>
                <form id="searchFlightsForm" onSubmit={this.handleSubmit}>
                    <div className="form-group has-warning">
                        <label htmlFor="originLocationCode">Origin location:</label>
                        <input id="originLocationCode" type="text" name="originLocationCode" maxLength="3" className="form-control text-uppercase" required />
                    </div>
                    <div className="form-group has-warning">
                        <label htmlFor="destinationLocationCode">Destination location:</label>
                        <input id="destinationLocationCode" type="text" name="destinationLocationCode" maxLength="3" className="form-control text-uppercase" required />
                    </div>
                    <div className="form-group has-warning">
                        <label htmlFor="departureDate">Departure:</label>
                        <input id="departureDate" name="departureDate" type="date" placeholder="Departure?" className="form-control" required />
                    </div>
                    <div className="form-group">
                        <label htmlFor="returnDate">Date of return:</label>
                        <input id="returnDate" name="returnDate" type="date" placeholder="Return?" className="form-control" />
                    </div>

                    <div className="form-group">
                        <label htmlFor="passengers">Passengers:</label>
                        <select id="passengers" name="passengers" className="form-control input-sm">
                            <option>1</option>
                            <option>2</option>
                            <option>3</option>
                            <option>4</option>
                            <option>5</option>
                        </select>
                    </div>
                    <div className="form-group">
                        <label htmlFor="currencyCode">Currency:</label>
                        <select id="currencyCode" name="currencyCode" className="form-control input-sm">
                            <option>USD</option>
                            <option>EUR</option>
                            <option>HRK</option>
                        </select>
                    </div>
                    <input type="submit" className="btn btn-default" value="Search" />
                    {loadingAreaText}
                </form>
                <br />
                {contents}
            </div>
        );
    }

    async populateFlightsData() {
        this.setState({  loading: true, firstLoad: false });
        var inputs = document.getElementById("searchFlightsForm").elements;
        var urlParams = new URLSearchParams({
            originLocationCode: inputs['originLocationCode'].value.toUpperCase(),
            destinationLocationCode: inputs['destinationLocationCode'].value.toUpperCase(),
            departureDate: inputs['departureDate'].value,
            returnDate: inputs['returnDate'].value,
            passengers: inputs['passengers'].value,
            currencyCode: inputs['currencyCode'].value
        });

        const response = await fetch('flights/get?' + urlParams);
        const data = await response.json();
        this.setState({ flights: data, loading: false, hasRecords: data.length > 0 });
    }
}
