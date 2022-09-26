import Login from "components/Login/Login";

const LoginPage: React.FC = () => {
    return (
        <div className='d-flex flex-row justify-content-center'>
            <div className='col-8 col-sm-6 col-md-4 col-lg-3'>
                <Login />
            </div>
        </div>
    );
}

export default LoginPage;