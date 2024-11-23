import  { Routes, Route } from 'react-router-dom';
import Login from './Component/Pages/Authentication/Login';
import { useAuth } from './hooks/auth-hook';
import UserInformationPage from './Component/UserInformationPage';
import NavigationBar from './Component/PageComponents/Navigationbar';
import SearchPage from './Component/SearchPage';
import { AuthContext } from './context/AuthContext';
import AdminPage from './Component/Pages/Admin/Adminpage';
import ListUsersPage from './Component/Pages/Users/ListUsersPage';
import AddUserPage from './Component/Pages/Users/AddUserPage';
import EditUserPage from './Component/Pages/Users/EditUserPage';
import ListClassPage from './Component/Pages/Classrooms/ListClassPage';
import CreateClassPage from './Component/Pages/Classrooms/CreateClassPage';
import EditClassPage from './Component/Pages/Classrooms/EditClassPage';
import ListCategoryPage from './Component/Pages/Category/ListCategoryPage';
import CreateCategoryPage from './Component/Pages/Category/CreateCategoryPage';
import EditCategoryPage from './Component/Pages/Category/EditCategoryPage';
import RequireAuth from './context/RequireAuth';
import UnauthorizedPage from './Component/Pages/Exceptions/UnauthorizedPage';
import NoAccessPage from './Component/Pages/Exceptions/NoAccessPage';
import ListItemsPage from './Component/Pages/ItemClassroom/ListItemsPage';
import CreateItemsPage from './Component/Pages/ItemClassroom/CreateItemsPage';
import EditItemsPage from './Component/Pages/ItemClassroom/EditItemsPage';
import AboutUsPage from './Component/Pages/AboutUsPage';
import { useState } from 'react';
import './index.css';
import ScannerPage from './Component/Pages/Scan/ScannerPage';
import CreateFromScanner from './Component/Pages/ItemClassroom/CreateFromScanner';
import SearchByCategory from './Component/Pages/Classrooms/SearchByCategory';
import UserPage from './Component/Pages/Users/UserPage';
function App() {
  const {token, login, logout, userId, role} = useAuth();
  const isAuth = !!token
  const [persist, setPersist] = useState(JSON.parse(localStorage.getItem('userData')) || false)

  return (
    <>
    <AuthContext.Provider value={{token, login, logout, userId, isAuth, role, persist, setPersist}}>
    <NavigationBar/>
    <Routes>
        <Route exact path="/" Component={Login} />
        <Route path='aboutus' Component={AboutUsPage}></Route>
        <Route path='unauthorized' Component={UnauthorizedPage}></Route>
        <Route path='noaccess' Component={NoAccessPage}></Route>
        <Route path="searchpage" Component={SearchPage} />
        

        <Route element={<RequireAuth allowedRoles={[0]}/>} >
        <Route path='admin/listusers' Component={ListUsersPage}/>
        <Route path='admin/adduser' Component={AddUserPage}/>
        <Route path='admin/edituser' Component={EditUserPage}/>
        </Route>

        <Route element={<RequireAuth allowedRoles={[0, 1]}/>} >
        <Route path='admin/createclass' Component={CreateClassPage}/>
        <Route path='admin/editclass/:id' Component={EditClassPage}/>
        </Route>

        <Route element={<RequireAuth allowedRoles={[0, 1, 2]}/>}>
        <Route path='admin/listclass' Component={ListClassPage}/>
        <Route path='admin/listcategory' Component={ListCategoryPage}/>
        <Route path='admin/createcategory' Component={CreateCategoryPage}/>
        <Route path='admin/editcategory/:id' Component={EditCategoryPage}/>
        <Route path='admin/adminpanel' Component={AdminPage} />
        <Route path='admin/listitems/:id' Component={ListItemsPage} />
        <Route path='admin/createitem/:id' Component={CreateItemsPage} />
        <Route path='admin/createnew/:number' Component={CreateFromScanner} />
        <Route path='admin/edititem/:id' Component={EditItemsPage} />
        <Route path="userinformationpage" Component={UserInformationPage} />
        <Route path='scan' Component={ScannerPage}/> 
        <Route path="admin/searchbycategory" Component={SearchByCategory} />
        <Route path="userinfo" Component={UserPage} />
        </Route>   
       
        
    </Routes>
    </AuthContext.Provider>
    </>
  );
}

export default App;
