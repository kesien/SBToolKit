import React, { Component } from 'react';

export class FetchData extends Component {
  static displayName = FetchData.name;

  constructor(props) {
    super(props);
      this.state = { courses: [], loading: true, courseinput: "hello" };
      this.search = this.search.bind(this);
  }

  componentDidMount() {
    this.populateWeatherData();
  }

  static renderForecastsTable(courses) {
    return (
      <table className='table table-striped' aria-labelledby="tabelLabel">
        <thead>
          <tr>
            <th>Coursenumber</th>
            <th>Language</th>
            <th>Level</th>
          </tr>
        </thead>
        <tbody>
          {courses.map(course =>
              <tr key={course.coursenumber}>
                  <td>{course.coursenumber}</td>
              <td>{course.language}</td>
              <td>{course.level}</td>
            </tr>
          )}
        </tbody>
      </table>
    );
  }

    async search() {
        this.setState({ loading: true });
        const response = await fetch('weatherforecast', {
            method: 'POST',
            body: JSON.stringify({ coursenumber: this.state.coursenumber })
        });
        const data = await response.json();
        this.setState({ courses: data, loading: false });
    }
  render() {
    let contents = this.state.loading
      ? <p><em>Loading...</em></p>
      : FetchData.renderForecastsTable(this.state.courses);

    return (
      <div>
        <h1 id="tabelLabel" >Weather forecast</h1>
            <p>This component demonstrates fetching data from the server.</p>
            <input type="text" value={this.state.courseinput} onChange={e => this.setState({ courseinput: e.target.value })} />
            <button onClick={this.search}>Search</button>
        {contents}
      </div>
    );
  }

  async populateWeatherData() {
    const response = await fetch('weatherforecast');
    const data = await response.json();
    this.setState({ courses: data, loading: false });
  }
}
