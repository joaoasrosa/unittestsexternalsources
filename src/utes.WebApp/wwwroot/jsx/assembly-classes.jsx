var Classes = React.createClass({
    getInitialState: function () {
        return { data: this.props.initialData };
    },
    render: function () {
        var classesComponent;
        if (this.state.data && this.state.data.length > 0) {
            classesComponent = <ClassesTable data={this.state.data } />;
        } else {
            classesComponent = <ClassesWarning />;
        }

        return (classesComponent);
    }
});

var ClassesWarning = React.createClass({
    render: function () {
        return (
            <div className="alert alert-info" role="alert"><strong>There is no classes in the assembly.</strong> Please use the Upload button to in the previous page.</div>
            );
    }
});

var ClassesTable = React.createClass({
    render: function () {
        return (
           <table className="table table-striped">
               <thead>
               <tr>
                   <th>Name</th>
                   <th>Full Name</th>
               </tr>
               </thead>
               <ClassesTableBody data={ this.props.data } />
           </table>
       );
    }
});

var ClassesTableBody = React.createClass({
    render: function () {
        const classRow = this.props.data.map(function (assemblyClass) {
            return (
                <tr key={assemblyClass.Name}>
                    <td><a href={'/Assembly/' + assemblyClass.AssemblyName + '/Class/' + assemblyClass.Name + '/Method' }>{assemblyClass.Name}</a></td>
                    <td>{assemblyClass.FullName}</td>
                </tr>
            );
        });
        return (
        <tbody>
            {classRow}
        </tbody>);
    }
});

ReactDOM.render(<Classes initialData={initialData} />, document.getElementById('classes'));