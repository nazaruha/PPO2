import "./scroll-logic.js";
import "./styles.css";
import { useEffect, useState } from "react";
import http from "../../../../http_common";
import { IProjectItem, IProjectServiceResponse, StoreProjectActionType } from "../types";
import { Link, useNavigate } from "react-router-dom";
import { store } from "../../../containers/store";

const ChooseProject = () => {
    const navigate = useNavigate();
    const [projects, setProjects] = useState<IProjectItem[]>([]);
    const [chosenProject, setChosenProject] = useState<IProjectItem>({
        id: -1,
        name: ""
    });

    useEffect(() => {
        http.get<IProjectServiceResponse>("api/project/projects")
            .then(resp => {
                const { payload } = resp.data;
                console.log(payload);
                setProjects(payload);
            }).catch(err => {
                console.log("ERR", err);
            })
    }, []);

    const viewList = projects.map((item) => (
        <tr key={item.id} className="project-item">
            <td
                className="project-data"
                onClick={() => onClickProject(item)}
                onDoubleClick={() => onDoubleClickProject(item)}
            >
                {item.name}
            </td>
        </tr>
    ))

    const onClickProject = (project: IProjectItem) => {
        setChosenProject({ id: project.id, name: project.name });
    }

    const onDoubleClickProject = (project: IProjectItem) => {
        console.log("Project clicked =>", project.id);
        window.localStorage.chosenProjectId = chosenProject.id;
        window.localStorage.chosenProjectName = chosenProject.name;
        store.dispatch({ type: StoreProjectActionType.STORE_CREATE_PROJECT, payload: chosenProject })
        console.log("Store project into the redux ChooseProject.tsx");
        navigate("/main-page");
    }

    return (
        <>
            <div className="container d-flex flex-column justify-content-center align-items-center">
                <h1 className="text-center">Оберіть Проект</h1>

                <div className="container mt-3 w-25 fs-5">
                    <table
                        id="dtVerticalScrollExample"
                        className="table table-bordered table-sm"
                        cellSpacing={0}
                        width="100%">
                        <thead>
                            <tr>
                                <th className="d-none"></th>
                            </tr>
                        </thead>
                        <tbody className="text-center">
                            <tr className="d-none">
                                <td></td>
                            </tr>
                            {viewList}
                        </tbody>
                    </table>
                    <div className="d-flex justify-content-center">
                        <Link className="btn btn-success fs-5" to="project-create">Створити новий <i className="fa fa-plus" aria-hidden="true"></i></Link>
                    </div>
                </div>
            </div>
        </>
    )
}

export default ChooseProject;