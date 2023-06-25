import { useEffect } from 'react';
import './App.css';
import { Routes, Route, useNavigate } from 'react-router-dom';
import ProjectChooseLayout from './components/containers/layouts/ProjectChooseLayout';
import ChooseProject from './components/pages/Project/ChooseProject';
import CreateProject from './components/pages/Project/CreateProject';
import DefaultLayout from './components/containers/default/DefaultLayout';
import MainPage from './components/pages/MainPage/MainPage';
import NotFoundPage from './components/pages/NotFound';
import { useSelector } from 'react-redux';
import { IStoreProject } from './components/pages/Project/types';
import ProjectProfile from './components/pages/Project/ProjectProfile';
import CustomerListPage from './components/pages/Customer/list/CustomerListPage';
import CustomerCreatePage from './components/pages/Customer/create/CustomerCreatePage';
import CustomerEditPage from './components/pages/Customer/edit/CustomerEditPage';
import ManufacturerListPage from './components/pages/Manufacturer/list/ManufacturerListPage';
import ManufacturerCreatePage from './components/pages/Manufacturer/create/ManufacturerCreatePage';
import ManufacturerEditPage from './components/pages/Manufacturer/edit/ManufacturerEditPage';
import ProductListPage from './components/pages/Product/list/ProductListPage';
import ProductCreatePage from './components/pages/Product/create/ProductCreate.Page';
import ProductEditPage from './components/pages/Product/edit/ProductEditPage';
import StorageListPage from './components/pages/Storage/list/StorageListPage';
import StorageEditPage from './components/pages/Storage/edit/StorageEditPage';
import StorageCreatePage from './components/pages/Storage/create/StorageCreatePage';
import OrderListPage from './components/pages/Order/list/OrderListPage';
import OrderCreatePage from './components/pages/Order/create/OrderCreatePage';
import OrderEditPage from './components/pages/Order/edit/OrderEditPage';

function App() {
  const navigate = useNavigate();
  const { isProjectStored, project } = useSelector((store: any) => store.project as IStoreProject);

  useEffect(() => {
    if (isProjectStored) {
      console.log("App.tsx project is chosen so move to the main-page");
      navigate("/main-page");
    }
  }, [])

  return (
    <>
      <Routes>
        <Route path="/" element={<ProjectChooseLayout />}>
          <Route index element={<ChooseProject />} />
          <Route path="project-create" element={<CreateProject />} />
        </Route>

        <Route path="/main-page" element={<DefaultLayout />}>
          <Route index element={<MainPage />} />
          <Route path="project">
            <Route index element={<MainPage />} />
            <Route path="profile" element={<ProjectProfile />} />
          </Route>
          <Route path="customer">
            <Route index element={<CustomerListPage />} />
            <Route path="create" element={<CustomerCreatePage />} />
            <Route path="edit/:id" element={<CustomerEditPage />} />
          </Route>
          <Route path="manufacturer">
            <Route index element={<ManufacturerListPage />} />
            <Route path="create" element={<ManufacturerCreatePage />} />
            <Route path="edit/:id" element={<ManufacturerEditPage />} />
          </Route>
          <Route path="product">
            <Route index element={<ProductListPage />} />
            <Route path="create" element={<ProductCreatePage />} />
            <Route path="edit/:productId/:manufacturerId" element={<ProductEditPage />} />
          </Route>
          <Route path="storage">
            <Route index element={<StorageListPage />} />
            <Route path="create" element={<StorageCreatePage />} />
            <Route path="edit/:productId/:manufacturerId/:projectId" element={<StorageEditPage />} />
          </Route>
          <Route path="order">
            <Route index element={<OrderListPage />} />
            <Route path="create" element={<OrderCreatePage />} />
            <Route path="edit/:id" element={<OrderEditPage />} />
          </Route>
        </Route>

        <Route path="*" element={<NotFoundPage />} />
      </Routes>
    </>
  );
}

export default App;
